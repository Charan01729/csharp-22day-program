using System.Globalization;
using System.Text;
using System.Security.Cryptography;
namespace EHRMvcAuditLedgerDemo.Services
{
    // Static service — no state, no DB access, no dependencies
    // Pure input → output: give it data, get a hash back
    // Real hospital: this logic would also be independently auditable by a third party
    // HIPAA requires that your tamper-detection mechanism itself can be verified by an external auditor
    public static class LedgerHashService
    {
        // Takes any string, returns its SHA-256 fingerprint as a 64-char hex string
        // Same input ALWAYS produces the same output — deterministic by design
        // Real hospital: in a full HIPAA system, you'd also HMAC-sign the hash using a secret key
        // stored in Azure Key Vault — so even if someone knows the algorithm, they can't forge a valid hash
        public static string Compute(string input)
        {
            // 'using' disposes the SHA256 object immediately after — cryptographic objects
            // hold unmanaged memory resources; not disposing them is a memory leak
            using var sha256 = SHA256.Create();

            // String → bytes (SHA-256 operates on bytes, not characters)
            var bytes = Encoding.UTF8.GetBytes(input);

            // ComputeHash → raw byte array → ToHexString → human-readable 64-char string
            // e.g., "A3F1C2...9B7D" — this is what gets stored in TransactionLedger.CurrentHash
            return Convert.ToHexString(sha256.ComputeHash(bytes));
        }

        // Recomputes the hash from live data and compares it to what was stored at recording time
        // Match = untouched. Mismatch = someone changed something after the fact.
        // Real hospital: this method runs on every compliance audit, not just on user request
        // Automated nightly integrity checks are a standard HIPAA operational safeguard
        public static bool VerifyChain(
            int transactionId,     // Ties the hash to a specific transaction — can't be reused across records
            string patientName,    // Ties the hash to a specific patient — name change = hash mismatch
            decimal amount,        // The financial figure being protected — even 1 cent difference breaks the hash
            string previousHash,   // Links this entry to the one before — deleting any entry breaks the entire chain
            string storedHash)     // The fingerprint stored in the DB at recording time — this is the ground truth
        {
            // Most common bug in hash verification: forgetting to normalize
            // 1000 vs 1000.0 vs 1000.00 are mathematically equal but produce three DIFFERENT hashes
            // "F2" = always 2 decimal places, InvariantCulture = always "." not ","
            // Real hospital: normalization rules must be documented and version-controlled
            // If the format ever changes, every historical hash becomes unverifiable permanently
            var normalizedAmount =
                amount.ToString("F2", CultureInfo.InvariantCulture);

            // Order and concatenation must be IDENTICAL to how Compute() was called during the original save
            // Adding a space, changing the order, or adding a new field retroactively
            // invalidates every single existing hash in the ledger — there is no undo
            // Real hospital: this input schema is treated like a protocol version
            // any change requires a migration strategy for existing ledger records
            var recalculatedHash = Compute(
                transactionId +
                patientName +
                normalizedAmount +
                previousHash
            );

            // Single comparison — true means the chain is intact, false means it was broken
            // Real hospital: a false result here triggers a HIPAA Breach Assessment
            // The clock starts immediately — 60 days to notify affected patients if PHI was compromised
            return recalculatedHash == storedHash;
        }
    }
}

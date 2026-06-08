<template>

  <h1>CareBridge Patients</h1>

  <div style="margin-bottom:20px">

    <label>Full Name:</label>

    <input
      v-model="FullName"
      type="text"
      placeholder="Enter text" />

    <label>City:</label>

    <input
      v-model="city"
      type="text"
      placeholder="Enter City" />

    <label>Status:</label>

    <select v-model="isActive">
      <option value="true">Active</option>
      <option value="false">Inactive</option>
    </select>

    <button @click="loadPatients">
      Search
    </button>
  <h3>Total Patients: {{ patients.length }}</h3>
  </div>

  <table border="1">

    <tr>
      <th>Patient Id</th>
      <th>Full Name</th>
      <th>City</th>
      <th>Active</th>
    </tr>

    <tr
      v-for="p in patients"
      :key="p.patientId">

      <td>{{ p.patientId }}</td>
      <td>{{ p.fullName }}</td>
      <td>{{ p.city }}</td>
      <td>{{ p.isActive }}</td>

    </tr>

  </table>

</template>



<script setup>
import { ref, onMounted } from 'vue'

const patients = ref([])
const city = ref('')
const isActive = ref('')
const FullName = ref('')

async function loadPatients() {

  let url = 'https://localhost:7062/api/patients'

  const params = new URLSearchParams()

  if (city.value.trim() !== '') {
    params.append('city', city.value)
  }

  if (isActive.value !== '') {
    params.append('isActive', isActive.value)
  }

    if (FullName.value !== '') {
    params.append('FullName', FullName.value)
  }

  if (params.toString()) {
    url += '?' + params.toString()
  }

  const response = await fetch(url)

  patients.value = await response.json()
}

onMounted(() => {
  loadPatients()
})
</script>


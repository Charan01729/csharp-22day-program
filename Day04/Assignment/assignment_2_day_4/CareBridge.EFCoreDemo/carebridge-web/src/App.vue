<template>

  <h1>Department Encounter Dashboard</h1>

  <table border="1">

    <tr>
      <th>Department Name</th>
      <th>In Patient</th>
      <th>Out Patient</th>
      <th>ED</th>
      <th>Total</th>
    </tr>

<tr
  v-for="(d, index) in department_load"
  :key="d.departmentName"
  :style="index === 0 ? 'background-color: yellow;' : ''">

  <td>{{ d.departmentName }}</td>
  <td>{{ d.inpatient }}</td>
  <td>{{ d.outpatient }}</td>
  <td>{{ d.ed }}</td>
  <td>{{ d.total }}</td>

</tr>

  </table>
<h2>Total Encounters: {{ grandTotal }}</h2>

</template>



<script setup>
import { ref, onMounted, computed } from 'vue'

const department_load = ref([])

const grandTotal = computed(() =>
  department_load.value.reduce(
    (sum, d) => sum + d.total,
    0
  )
)

async function load_encounter_info() {

  let url = 'https://localhost:7062/api/analytics/department-load'


  const response = await fetch(url)

  department_load.value = await response.json()
}

onMounted(() => {
  load_encounter_info()
})
</script>


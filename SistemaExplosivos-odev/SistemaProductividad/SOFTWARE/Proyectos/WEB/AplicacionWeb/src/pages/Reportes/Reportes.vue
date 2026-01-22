<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Reportes</div>
    </div>

    <q-card>
      <q-card-section>
        <div class="text-subtitle1 text-weight-medium">Exportación</div>
        <div class="text-caption text-textsecondary">
          Genera reportes PDF, Excel o CSV por departamento.
        </div>
      </q-card-section>
      <q-separator />
      <q-card-section class="row q-col-gutter-md items-center">
        <div class="col-12 col-md-4">
          <q-input
            v-model.number="departamentoId"
            type="number"
            min="1"
            label="ID de departamento"
            dense
            filled
          />
        </div>
        <div class="col-12 col-md-8 row q-col-gutter-sm">
          <div class="col-12 col-sm-auto">
            <q-btn color="primary" icon="picture_as_pdf" label="PDF" @click="exportar('pdf')" />
          </div>
          <div class="col-12 col-sm-auto">
            <q-btn color="primary" icon="table_view" label="Excel" @click="exportar('excel')" />
          </div>
          <div class="col-12 col-sm-auto">
            <q-btn color="primary" icon="table_chart" label="CSV" @click="exportar('csv')" />
          </div>
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { api } from 'boot/axios'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const departamentoId = ref(1)

const exportar = async (tipo) => {
  try {
    let endpoint = '/reportes/ExportarPDF'
    let nombre = `Reporte_${departamentoId.value}`

    if (tipo === 'excel') {
      endpoint = '/reportes/ExportarExcel'
      nombre = `${nombre}.xlsx`
    } else if (tipo === 'csv') {
      endpoint = '/reportes/ExportarCSV'
      nombre = `${nombre}.csv`
    } else {
      nombre = `${nombre}.pdf`
    }

    const response = await api.post(endpoint, null, {
      params: tipo === 'pdf' ? { departamentoId: departamentoId.value } : undefined,
      responseType: 'blob',
    })

    const href = URL.createObjectURL(response.data)
    const link = document.createElement('a')
    link.href = href
    link.setAttribute('download', nombre)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    URL.revokeObjectURL(href)
  } catch (error) {
    console.error(error)
    $q.notify({ type: 'negative', message: 'No se pudo generar el reporte.' })
  }
}
</script>

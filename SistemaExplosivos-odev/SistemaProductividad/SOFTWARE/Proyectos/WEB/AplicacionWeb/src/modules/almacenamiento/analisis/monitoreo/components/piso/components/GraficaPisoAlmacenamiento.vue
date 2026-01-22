<template>
  <div ref="chartRef" class="row justify-center grafica"></div>
</template>

<script setup>
import { onBeforeUnmount, onMounted, ref, toRefs, watch } from 'vue'
import * as echarts from 'echarts'
import { colors, date, getCssVar } from 'quasar'

const chartRef = ref(null)
let chartInstance = null
let resizeObserver = null
let themeObserver = null

const props = defineProps({
  datosGrafico: {
    type: Array,
    default: () => [],
  },
  color: {
    type: String,
    default: '',
  },
})

const { datosGrafico, color } = toRefs(props)

const initChart = () => {
  if (chartInstance) {
    chartInstance.dispose()
  }
  chartInstance = echarts.init(chartRef.value)
  updateChart()
}

const updateChart = () => {
  if (!chartInstance) return

  const primary = color.value || getCssVar('primary')
  const lightPrimary = colors.changeAlpha(primary, 0.25)
  const now = new Date()

  const option = {
    tooltip: {
      trigger: 'axis',
      formatter: (params) => {
        const p = params[0]
        return `<b>${p.axisValue}</b><br/>${p.value.toLocaleString()} kg`
      },
      confine: true,
    },
    grid: {
      left: '0',
      right: '0',
      bottom: '0',
      top: '20px',
      containLabel: true,
    },
    xAxis: {
      type: 'category',
      boundaryGap: true,
      data: datosGrafico.value.map((x) => x.nombreTiempo),
      axisTick: { show: false },
      axisLine: { show: false },
      axisLabel: {
        fontSize: 12,
        interval: 0,
      },
      axisPointer: {
        type: 'shadow', // Highlight category on hover
      },
    },
    yAxis: {
      type: 'value',
      show: false,
    },
    series: [
      {
        type: 'bar',
        barWidth: '50%',
        data: datosGrafico.value.map((item) => {
          const isToday =
            date.formatDate(item.fechaHora, 'YYYY-MM-DD') === date.formatDate(now, 'YYYY-MM-DD')

          return {
            value: item.valorMovimiento,
            itemStyle: {
              borderRadius: [6, 6, 0, 0],
              color: isToday ? primary : lightPrimary,
            },
          }
        }),
      },
    ],
  }

  chartInstance.setOption(option)
}

watch(datosGrafico, () => {
  updateChart()
})

onMounted(() => {
  initChart()

  // Detect size changes
  resizeObserver = new ResizeObserver(() => {
    chartInstance?.resize()
  })

  // Detect theme/color changes on the document root
  themeObserver = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
      if (mutation.type === 'attributes' && mutation.attributeName === 'style') {
        updateChart()
      }
    })
  })

  if (chartRef.value) {
    resizeObserver.observe(chartRef.value)
  }

  // Observe styles on documentElement for CSS variable changes
  themeObserver.observe(document.documentElement, {
    attributes: true,
    attributeFilter: ['style'],
  })
})

onBeforeUnmount(() => {
  if (resizeObserver) {
    resizeObserver.disconnect()
  }
  if (themeObserver) {
    themeObserver.disconnect()
  }
  chartInstance?.dispose()
})
</script>

<style scoped>
.grafica {
  width: 100%;
  height: 100%;
  min-height: 180px; /* Altura mínima para asegurar visibilidad */
}
</style>

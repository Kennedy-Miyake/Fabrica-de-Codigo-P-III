<!-- src/components/BarcodeScanner.vue -->
<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Quagga from 'quagga'

const cameraEl = ref(null)
const code     = ref('')

function start () {
  if (!cameraEl.value) return
  Quagga.init(
    {
      inputStream: {
        type: 'LiveStream',
        target: cameraEl.value,
        constraints: { facingMode: 'environment' }
      },
      decoder: { readers: ['ean_reader', 'code_128_reader', 'upc_reader'] }
    },
    err => { if (!err) Quagga.start() }
  )
  Quagga.onDetected(data => {
    code.value = data.codeResult.code
    Quagga.stop()
  })
}

onMounted(start)
onBeforeUnmount(() => Quagga.stop())
</script>

<template>
  <div>
    <div ref="cameraEl" class="relative w-full aspect-video bg-black">
      <div class="absolute inset-y-0 left-1/2 w-px bg-red-500/60 -translate-x-1/2"></div>
    </div>
    <p class="mt-4 text-center font-semibold">
      <span v-if="code">✅ Código: {{ code }}</span>
      <span v-else>⌛ Aguardando…</span>
    </p>
  </div>
</template>

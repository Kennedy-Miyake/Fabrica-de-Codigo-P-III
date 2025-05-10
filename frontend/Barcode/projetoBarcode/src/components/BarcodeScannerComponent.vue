<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Quagga from 'quagga'

const cameraEl = ref(null)
const code     = ref('')
const error    = ref('')

function startScanner() {
  if (!cameraEl.value) return

  Quagga.init(
    {
      inputStream: {
        type: 'LiveStream',
        target: cameraEl.value,
        constraints: {
          facingMode: 'environment',
          width:  { ideal: 800 },
          height: { ideal: 300 }
        }
      },
      decoder: {
        readers: ['ean_reader']  // ← somente EAN-13
      },
      locate: false,             // ← desativa o locate
      frequency: 5,              // reduz leituras por segundo
      numOfWorkers: 1            // força 1 worker, evita conflitos
    },
    err => {
      if (err) {
        console.error(err)
        error.value = 'Não foi possível iniciar a câmera'
        return
      }
      Quagga.start()
    }
  )

  Quagga.onDetected(data => {
    const result = data.codeResult
    // só aceite se for realmente EAN-13 (13 dígitos)
    if (result.format === 'ean_13' && result.code.length === 13) {
      code.value = result.code
      Quagga.stop()
    }
  })
}

onMounted(startScanner)
onBeforeUnmount(() => Quagga.stop())
</script>

<template>
  <div class="mx-auto w-[450px] h-[100px] bg-black rounded-md overflow-hidden relative" ref="cameraEl">
    <div class="absolute inset-x-0 top-1/2 h-px bg-red-500/70"></div>
  </div>

  <p class="mt-3 text-center font-semibold">
    <span v-if="code">✅ Código lido: {{ code }}</span>
    <span v-else-if="error">❌ {{ error }}</span>
    <span v-else>⌛ Aguardando leitura…</span>
  </p>
</template>

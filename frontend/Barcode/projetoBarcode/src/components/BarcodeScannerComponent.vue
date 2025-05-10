<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Quagga from 'quagga'

/* elemento para renderizar a câmera */
const cameraEl = ref<HTMLDivElement | null>(null)

/* linha-guia visível */
const guideEl  = ref<HTMLDivElement | null>(null)

/* estado do resultado (exibido na tela ou emitido via evento) */
const code     = ref('')

/* inicia a câmera e o scanner */
function startScanner() {
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
    err => {
      if (err) return console.error(err)
      Quagga.start()
    }
  )

  /* evento disparado quando encontra código válido */
  Quagga.onDetected(data => {
    code.value = data.codeResult.code
    // exemplo: parar e emitir evento para quem usar o componente
    Quagga.stop()
    /* this.$emit('detected', code.value)  // se preferir emitir */
  })
}

onMounted(startScanner)
onBeforeUnmount(() => Quagga.stop())
</script>

<template>
  <div class="w-full">
    <!-- área da câmera -->
    <div ref="cameraEl" class="relative w-full aspect-video overflow-hidden bg-black">
      <!-- linha-guia -->
      <div
        ref="guideEl"
        class="absolute left-1/2 top-1/2 h-full w-px -translate-x-1/2 -translate-y-1/2
               bg-red-500/60 pointer-events-none"
      ></div>
    </div>

    <!-- resultado embaixo -->
    <p class="mt-4 text-center text-lg font-semibold">
      <span v-if="code">✅ Código lido: {{ code }}</span>
      <span v-else>⌛ Aguardando leitura…</span>
    </p>
  </div>
</template>

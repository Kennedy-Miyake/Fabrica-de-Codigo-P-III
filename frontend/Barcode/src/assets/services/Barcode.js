import Quagga from 'quagga'
import { useRouter } from 'vue-router'

export default function initBarcodeScanner(router) { // Recebe router como parâmetro
  Quagga.init({
    inputStream: {
      type: 'LiveStream',
      constraints: {
        width: 640,
        height: 480,
        facingMode: 'environment'
      },
      target: document.querySelector('#camera')
    },
    locator: {
      patchSize: 'medium',
      halfSample: true
    },
    numOfWorkers: navigator.hardwareConcurrency || 4,
    frequency: 10, // maior valor = menor frequência de escaneamento (evita leituras duplicadas)
    decoder: {
      readers: ['ean_reader'],
      multiple: false
    },
    locate: true,
  }, (err) => {
    if (err) {
      console.error(err)
      return
    }
    Quagga.start()
  })

  let lastCode = null
  Quagga.onDetected((result) => {
    const code = result.codeResult.code

    if (code !== lastCode) {
      lastCode = code
      const resultado = document.getElementById('resultado')
      if (resultado) resultado.textContent = `✅ Código lido: ${code}`

      // Corrigindo o redirecionamento para usar o endpoint correto
      router.push(`/products/${code}`)

      // Parar o scanner após detecção bem sucedida
      Quagga.stop()
    }
  })
}

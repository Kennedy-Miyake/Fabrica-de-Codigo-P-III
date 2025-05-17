import Quagga from 'quagga'

export default function initBarcodeScanner() {
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
      patchSize: 'medium', // opções: x-small, small, medium, large, x-large
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

    //função onde verifica se codigo foi lido
    if (code !== lastCode) {
      lastCode = code
      const resultado = document.getElementById('resultado')
      if (resultado) resultado.textContent = `✅ Código lido: ${code}`

      //passando barcode na url 
      window.location.href = `/product?code=${code}`
    }
  })
}

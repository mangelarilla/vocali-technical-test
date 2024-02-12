## Prueba tecnica VOCALI

`FakeTranscriptionService` -> Imita el servicio de transcripcion, escribiendo el .txt de vuelta, la parte del login no se muy a que se refiere

`FileSenderService` -> El servicio de envio, tengo que decir como extra que a nivel de requisitos a mi claramente me dice Azure Function con un blob storage, es el caso de uso perfecto.

`FileSenderService.Tests` -> Un par de Unit Tests para el servicio, he preferido testear la logica propia y dejar de lado las librerias ya testeadas, para cubrir esos casos iria mejor otro proyecto de AcceptanceTests con algo como testcontainers o similar
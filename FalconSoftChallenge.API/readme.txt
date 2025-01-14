ENDPOINTS:

La WebAPI tiene Swagger configurado.

Hay 2 users creados por defecto para usar la aplicacion, son los siguientes:

augusto@server.com / racingcampeon2024
federico@server.com / 123456

Los passwords se están guardando en texto plano, no utilicé ningún metodo de encriptación dado que esto es un challenge.

Con estas credenciales pueden pegarle al siguiente endpoint:

POST /Auth/Login - 

Ejemplo de Request

{
  "email": "string",
  "password": "string"
}

Luego con el token generado pueden utilizar los 2 endpoints restantes.

El 1ro, retorna el listado de ordenes paginado y recibe parametros para poder filtrar y ordenar los resultados.

GET /Orders 

---

El 2do endpoint, permite modificar el listado de productos asociado a una orden, añadiendo nuevos, modificando la cantidad de un producto en la orden o eliminandolo de la orden.
Esto fue muy subjetivo para mi, porque el enunciado decia "Crear un endpoint que modifique algun campo del detalle de la orden", y YO entendi que el detalle eran los productos. Por lo tanto,
si yo editaba los productos desde una orden estos cambios iban a impactar en todas las ordenes.
Entonces lo que hice fue relacionar productos con ordenes permitiendo elegir la cantidad de un producto especifico dentro de una orden.

Un ejemplo de un request podria ser el siguiente:

REQUEST PREVIO

{
      "products": [
        {
          "id": "490d2e31-f913-4d13-990f-3d04d817ff9f",
          "name": "Bicicleta",
          "price": 5000,
          "quantity": 2
        },
        {
          "id": "d6f49ccc-8dc6-4718-9c1d-63d724784718",
          "name": "Pelota",
          "price": 10,
          "quantity": 2
        }
      ]
}

NUEVO REQUEST

{
      "products": [
        {
          "id": "490d2e31-f913-4d13-990f-3d04d817ff9f",
          "name": "Bicicleta",
          "price": 5000,
          "quantity": 3
        },
        {
          "id": "d6f49ccc-8dc6-4718-9c1d-63d724784718",
          "name": "Pelota",
          "price": 10,
          "quantity": 0
        }
      ]
}

Por ejemplo, aca se podria cambiar el valor de Quantity para el primer producto a 3 por.
Tambien se podria poner Quantity en 0 para el 2do producto, lo que haria que ese producto deje de estar vinculado con esa orden.

Estos cambios también impactarian en la orden actualizando el valor del campo "Amount".

No sé si hice bien o mal con la interpretación del enunciado pero me pareció lo más correcto cuando se trabaja con API REST.

----

Gracias por la oportunidad!
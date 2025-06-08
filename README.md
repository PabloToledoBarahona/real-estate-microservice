# FavoritesService
Microservicio de gestión de favoritos e historial de visitas para propiedades inmobiliarias.

---

## Descripción

Este microservicio permite a los usuarios autenticados:

- Marcar propiedades como favoritas.
- Obtener la lista de propiedades favoritas.
- Registrar automáticamente visitas a propiedades.
- Filtrar el historial de visitas por fecha, tipo o transacción.
- Generar criterios de recomendación basados en historial.
- Consultar estadísticas de visitas por día y por zona.

Está desarrollado en .NET 7 y utiliza Cassandra como base de datos distribuida.

---

## Requisitos Funcionales Implementados

| Código | Requisito |
|--------|-----------|
| RF3.1 | Permitir al usuario marcar una propiedad como favorita. |
| RF3.2 | Mostrar una lista de propiedades favoritas del usuario autenticado. |
| RF3.3 | Registrar automáticamente las propiedades visitadas por el usuario. |
| RF3.4 | Permitir filtrar el historial de propiedades por fecha de visita o tipo. |
| RF3.5 | Generar recomendaciones basadas en el historial de visualización. |
| RF3.6 | Registrar fecha y hora de cada visita. |
| RF3.7 | Permitir ver las propiedades más visitadas por día y zona. |

---

# Endpoints

## Favoritos

- POST /favorites  
  Marca una propiedad como favorita del usuario actual.

- GET /favorite  
  Lista las propiedades favoritas del usuario autenticado.

## Historial de Visitas

- POST /visits  
  Registra una visita a una propiedad (automática).

- GET /visits  
  Devuelve todo el historial de visitas del usuario.

- GET /visits/filter  
  Filtra visitas por fecha, tipo de propiedad o tipo de transacción.

- GET /visits/stats/{propertyId}  
  Devuelve estadísticas de visitas diarias y por ciudad para una propiedad.

- GET /visits/recommendations  
  Devuelve los criterios más frecuentes del usuario para futuras recomendaciones.

---

## Tablas Cassandra

1. favorites_by_user  
   Almacena las propiedades marcadas como favoritas por cada usuario.

2. visit_history_by_user  
   Historial de visitas realizadas por cada usuario con timestamp.

3. property_visit_count_by_day  
   Contador de visitas por propiedad y por día.

4. property_visit_count_by_zone  
   Contador de visitas por ciudad, propiedad y fecha.

---

## Autenticación

Este microservicio requiere autenticación mediante JWT. Se espera que el token incluya el identificador del usuario (sub o nameidentifier) en su payload.

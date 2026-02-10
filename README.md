# GNB Backend

API REST para gestión de transacciones y conversión de divisas.

## Requisitos

- .NET 8 SDK

## Ejecución
```bash
dotnet run --project src/GnbBackend.Api
```

Abrir: https://localhost:5001/swagger

## Tests
```bash
dotnet test
```

## Endpoints

- `GET /api/transactions` - Todas las transacciones
- `GET /api/rates` - Tipos de cambio
- `GET /api/sku/{sku}` - Detalle de SKU convertido a EUR

Ejemplo: `/api/sku/T20061` devuelve total en EUR (13.06)

## Estructura
```
src/
  GnbBackend.Core/     # Lógica de negocio
  GnbBackend.Api/      # Controladores y API
tests/
  GnbBackend.Tests/    # Tests unitarios
Data/                  # JSON con datos
```

## Decisiones importantes

**Conversión de monedas:** Usa BFS para encontrar rutas entre divisas. Por ejemplo, SEK -> USD -> EUR si no hay conversión directa SEK-EUR.

**Redondeo:** Banker's rounding (a 2 decimales) después de cada conversión para minimizar errores acumulativos.

**Tipo decimal:** Para evitar problemas de precisión con dinero (no usar double/float).

**Monedas sin conversión:** Si no hay ruta (como BRL en el ejemplo), se reporta en warnings y se excluye del total.

## Notas

- Los importes negativos se manejan normalmente (devoluciones, ajustes)
- Cache de datos JSON en memoria
- Dependency injection para testabilidad

## Autor
Juan Camilo Rayo Castillo
Desarrollado como prueba técnica para Voltec.
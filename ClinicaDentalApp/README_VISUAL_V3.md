# Mejoras visuales V3

Esta versión mantiene la funcionalidad existente y aplica únicamente ajustes visuales en Razor/CSS.

## Cambios principales

- Login más limpio y balanceado para evitar que la imagen del doctor domine toda la pantalla.
- Reducción de información visual en el login: menos elementos flotantes y más foco en el formulario.
- Implementación de iconos outline con Lucide Icons mediante CDN.
- Reemplazo de emojis por iconos de borde en:
  - Login
  - Sidebar / menú responsive
  - Página principal
  - Botones de edición en citas y pacientes
  - Estado vacío de pacientes y ventas
- Página principal más limpia, con menos texto y accesos rápidos claros.
- Animaciones suaves en login, tarjetas y módulos.
- Ajustes CSS para que el login no genere scroll innecesario en pantallas de escritorio.

## Archivos modificados

- `ClinicaDental.Web/Pages/Auth/Login.cshtml`
- `ClinicaDental.Web/Pages/Shared/_Layout.cshtml`
- `ClinicaDental.Web/Pages/Index.cshtml`
- `ClinicaDental.Web/Pages/Citas/Index.cshtml`
- `ClinicaDental.Web/Pages/Pacientes/Index.cshtml`
- `ClinicaDental.Web/Pages/Ventas/Index.cshtml`
- `ClinicaDental.Web/wwwroot/css/site.css`

## Importante

No se modificaron:

- Controladores
- Modelos
- Repositorios
- Servicios
- Archivos `.cshtml.cs`
- Conexión a base de datos
- Lógica de autenticación

## Nota sobre iconos

Lucide Icons se carga desde CDN:

```html
<script src="https://unpkg.com/lucide@latest/dist/umd/lucide.min.js"></script>
```

El proyecto ya usaba Tailwind por CDN, así que esta mejora mantiene el mismo enfoque visual sin agregar paquetes de Node/NPM.

import { sleep } from 'k6';
import { check } from 'k6';
import { login, authGet, parseJson } from './shared.js';

export const options = {
  vus: 1,
  iterations: 1,
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<1200'],
  },
};

export function setup() {
  return { token: login() };
}

export default function (data) {
  const endpoints = [
    '/api/catalogos/roles',
    '/api/catalogos/estados-cita',
    '/api/catalogos/proveedores',
    '/api/catalogos/tipos-movimiento-inventario',
    '/api/catalogos/categorias-servicio',
    '/api/catalogos/estados-venta',
    '/api/catalogos/metodos-pago',
    '/api/catalogos/usuarios-activos',
  ];

  for (const endpoint of endpoints) {
    const response = authGet(endpoint, data.token);
    const body = parseJson(response);
    check(response, {
      [`${endpoint} devuelve arreglo`]: () => Array.isArray(body),
    });
    sleep(0.2);
  }
}

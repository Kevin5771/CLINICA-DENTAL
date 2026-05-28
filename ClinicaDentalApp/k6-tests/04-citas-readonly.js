import { sleep } from 'k6';
import { check } from 'k6';
import { login, authGet, parseJson, todayPlusDays } from './shared.js';

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
  const fechaDesde = todayPlusDays(0);
  const fechaHasta = todayPlusDays(30);
  const response = authGet(`/api/citas?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`, data.token);
  const body = parseJson(response);

  check(response, {
    'citas devuelve arreglo': () => Array.isArray(body),
  });

  if (Array.isArray(body) && body.length > 0) {
    const id = body[0].idCita || body[0].IdCita;
    if (id) {
      const detail = authGet(`/api/citas/${id}`, data.token);
      check(detail, {
        'detalle de cita responde 200': (r) => r.status === 200,
      });
    }
  }

  sleep(1);
}

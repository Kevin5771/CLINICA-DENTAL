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
  const response = authGet('/api/pacientes', data.token);
  const body = parseJson(response);

  check(response, {
    'pacientes devuelve arreglo': () => Array.isArray(body),
  });

  if (Array.isArray(body) && body.length > 0) {
    const id = body[0].idPaciente || body[0].IdPaciente;
    if (id) {
      const detail = authGet(`/api/pacientes/${id}`, data.token);
      check(detail, {
        'detalle de paciente responde 200': (r) => r.status === 200,
      });
    }
  }

  sleep(1);
}

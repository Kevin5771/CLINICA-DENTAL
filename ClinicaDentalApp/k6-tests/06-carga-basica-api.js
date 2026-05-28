import { sleep } from 'k6';
import { login, authGet } from './shared.js';

export const options = {
  stages: [
    { duration: '10s', target: Number(__ENV.VUS || 5) },
    { duration: '20s', target: Number(__ENV.VUS || 5) },
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    http_req_failed: ['rate<0.05'],
    http_req_duration: ['p(95)<2000'],
  },
};

export function setup() {
  return { token: login() };
}

export default function (data) {
  authGet('/api/pacientes', data.token);
  authGet('/api/citas', data.token);
  authGet('/api/catalogos/estados-cita', data.token);
  authGet('/api/stock/resumen', data.token);
  authGet('/api/reportes/resumen', data.token);
  sleep(1);
}

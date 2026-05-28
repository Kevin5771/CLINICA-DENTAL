import { sleep } from 'k6';
import { check } from 'k6';
import http from 'k6/http';
import { BASE_URL, API_USER, API_PASSWORD, jsonHeaders, parseJson } from './shared.js';

export const options = {
  vus: 1,
  iterations: 1,
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<1000'],
  },
};

export default function () {
  const response = http.post(
    `${BASE_URL}/api/auth/login`,
    JSON.stringify({ usuario: API_USER, password: API_PASSWORD }),
    jsonHeaders()
  );

  const body = parseJson(response);

  check(response, {
    'login status 200': (r) => r.status === 200,
    'login trae token': () => Boolean(body && body.token),
    'login trae usuario': () => Boolean(body && body.username),
    'login trae rol': () => Boolean(body && body.rol),
  });

  sleep(1);
}

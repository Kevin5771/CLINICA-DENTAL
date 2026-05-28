import http from 'k6/http';
import { check, fail } from 'k6';

export const BASE_URL = (__ENV.BASE_URL || 'http://localhost:5000').replace(/\/$/, '');
export const API_USER = __ENV.API_USER || 'admin';
export const API_PASSWORD = __ENV.API_PASSWORD || 'Admin123*';

export function jsonHeaders(token = null) {
  const headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  };

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  return { headers };
}

export function parseJson(response) {
  try {
    return response.json();
  } catch (_) {
    return null;
  }
}

export function login() {
  const payload = JSON.stringify({
    usuario: API_USER,
    password: API_PASSWORD,
  });

  const response = http.post(`${BASE_URL}/api/auth/login`, payload, jsonHeaders());
  const body = parseJson(response);

  const ok = check(response, {
    'login responde 200': (r) => r.status === 200,
    'login devuelve token JWT': () => Boolean(body && body.token),
  });

  if (!ok) {
    fail(`No se pudo iniciar sesión. Verifica BASE_URL, API_USER y API_PASSWORD. Status: ${response.status}. Body: ${response.body}`);
  }

  return body.token;
}

export function authGet(path, token, expectedStatus = 200) {
  const response = http.get(`${BASE_URL}${path}`, jsonHeaders(token));
  check(response, {
    [`GET ${path} responde ${expectedStatus}`]: (r) => r.status === expectedStatus,
  });
  return response;
}

export function authPost(path, token, payload, acceptedStatuses = [200, 201]) {
  const response = http.post(`${BASE_URL}${path}`, JSON.stringify(payload), jsonHeaders(token));
  check(response, {
    [`POST ${path} responde ${acceptedStatuses.join(' o ')}`]: (r) => acceptedStatuses.includes(r.status),
  });
  return response;
}

export function todayPlusDays(days) {
  const date = new Date();
  date.setDate(date.getDate() + days);
  return date.toISOString().substring(0, 10);
}

export function randomCode(prefix) {
  const random = Math.floor(Math.random() * 1000000).toString().padStart(6, '0');
  return `${prefix}${__VU || 1}${__ITER || 0}${random}`.substring(0, 20);
}

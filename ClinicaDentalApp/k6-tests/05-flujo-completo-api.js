import { sleep } from 'k6';
import { check, fail } from 'k6';
import { login, authGet, authPost, parseJson, todayPlusDays, randomCode } from './shared.js';

export const options = {
  vus: 1,
  iterations: 1,
  thresholds: {
    http_req_failed: ['rate<0.02'],
    http_req_duration: ['p(95)<1500'],
  },
};

export function setup() {
  return { token: login() };
}

function firstId(items, camelName, pascalName) {
  if (!Array.isArray(items) || items.length === 0) return null;
  return items[0][camelName] || items[0][pascalName] || null;
}

export default function (data) {
  const token = data.token;

  const pacientesAntes = parseJson(authGet('/api/pacientes', token));
  check({ pacientesAntes }, {
    'pacientes iniciales es arreglo': (x) => Array.isArray(x.pacientesAntes),
  });

  const pacientePayload = {
    codigoPaciente: randomCode('K6P'),
    nombres: 'Paciente',
    apellidos: 'Prueba Automatizada',
    telefono: '55551234',
    fechaNacimiento: '1999-01-15',
    genero: 'Masculino',
    direccion: 'Guatemala',
    correo: `paciente.k6.${Date.now()}@demo.local`,
    alergias: 'Ninguna',
    observacionesGenerales: 'Paciente creado por prueba end-to-end k6.',
  };

  const crearPacienteResponse = authPost('/api/pacientes', token, pacientePayload, [201]);
  const pacienteCreado = parseJson(crearPacienteResponse);
  const idPaciente = pacienteCreado && (pacienteCreado.idPaciente || pacienteCreado.IdPaciente);

  check(crearPacienteResponse, {
    'crear paciente devuelve id': () => Boolean(idPaciente),
  });

  if (!idPaciente) {
    fail(`No se pudo obtener idPaciente. Body: ${crearPacienteResponse.body}`);
  }

  const usuarios = parseJson(authGet('/api/catalogos/usuarios-activos', token));
  const estadosCita = parseJson(authGet('/api/catalogos/estados-cita', token));
  const idDentista = firstId(usuarios, 'idUsuario', 'IdUsuario');
  const idEstadoCita = firstId(estadosCita, 'idEstadoCita', 'IdEstadoCita') || 1;

  check({ idDentista, idEstadoCita }, {
    'existe usuario activo para crear cita': (x) => Boolean(x.idDentista),
    'existe estado de cita': (x) => Boolean(x.idEstadoCita),
  });

  if (!idDentista) {
    fail('No hay usuarios activos en catálogo para crear la cita. Agrega un usuario/dentista activo en la base de datos.');
  }

  const citaPayload = {
    idPaciente,
    idDentista,
    fecha: todayPlusDays(3),
    horaInicio: '09:00:00',
    horaFin: '09:30:00',
    motivo: 'Evaluación general k6',
    observaciones: 'Cita creada por prueba end-to-end k6.',
    idEstadoCita,
    creadaPor: idDentista,
  };

  const crearCitaResponse = authPost('/api/citas', token, citaPayload, [201]);
  const citaCreada = parseJson(crearCitaResponse);
  const idCita = citaCreada && (citaCreada.idCita || citaCreada.IdCita);

  check(crearCitaResponse, {
    'crear cita devuelve id': () => Boolean(idCita),
  });

  if (idCita) {
    authGet(`/api/citas/${idCita}`, token);
  }

  authGet('/api/stock/resumen', token);
  authGet('/api/reportes/resumen', token);

  sleep(1);
}

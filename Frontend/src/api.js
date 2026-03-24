const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5248';

async function request(method, path, body) {
  const res = await fetch(`${BASE_URL}${path}`, {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: body ? JSON.stringify(body) : undefined,
  });
  const data = await res.json().catch(() => ({}));
  if (!res.ok) throw new Error(data.message || `Error ${res.status}`);
  return data;
}

export const api = {
  // Parents
  createParent: (body) => request('POST', '/api/parents', body),
  getParent: (id) => request('GET', `/api/parents/${id}`),
  getParents: () => request('GET', '/api/parents'),

  // Students
  createStudent: (body) => request('POST', '/api/students', body),
  getStudent: (id) => request('GET', `/api/students/${id}`),
  getStudents: () => request('GET', '/api/students'),

  // Classes
  createClass: (body) => request('POST', '/api/classes', body),
  getClasses: (day) => request('GET', `/api/classes${day ? `?day=${day}` : ''}`),

  // Registrations
  registerStudent: (classId, body) => request('POST', `/api/classes/${classId}/register`, body),
  cancelRegistration: (id) => request('DELETE', `/api/registrations/${id}`),

  // Subscriptions
  createSubscription: (body) => request('POST', '/api/subscriptions', body),
  getSubscription: (id) => request('GET', `/api/subscriptions/${id}`),
  getSubscriptionsByStudent: (studentId) => request('GET', `/api/subscriptions?studentId=${studentId}`),
  useSession: (id) => request('PATCH', `/api/subscriptions/${id}/use`),
};

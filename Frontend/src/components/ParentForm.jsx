import { useState } from 'react';
import { api } from '../api';

export default function ParentForm({ onCreated }) {
  const [form, setForm] = useState({ name: '', phone: '', email: '' });
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const data = await api.createParent(form);
      setResult(data);
      onCreated?.(data);
      setForm({ name: '', phone: '', email: '' });
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="card">
      <h3>Tạo Phụ Huynh</h3>
      <form onSubmit={handleSubmit}>
        <input placeholder="Họ tên" value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} required />
        <input placeholder="Số điện thoại" value={form.phone} onChange={e => setForm({ ...form, phone: e.target.value })} required />
        <input placeholder="Email" type="email" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} required />
        <button type="submit">Tạo</button>
      </form>
      {error && <p className="error">{error}</p>}
      {result && <p className="success">✓ Tạo thành công — ID: <code>{result.id}</code></p>}
    </div>
  );
}

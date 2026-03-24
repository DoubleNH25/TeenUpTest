import { useState } from 'react';
import ParentForm from './components/ParentForm';
import StudentForm from './components/StudentForm';
import ClassSchedule from './components/ClassSchedule';
import RegisterForm from './components/RegisterForm';
import SubscriptionPanel from './components/SubscriptionPanel';
import CancelRegistration from './components/CancelRegistration';
import './App.css';

const TABS = [
  { key: 'schedule', label: '📅 Lịch Lớp & Đăng Ký' },
  { key: 'people', label: '👤 Phụ Huynh & Học Sinh' },
  { key: 'subscription', label: '📦 Gói Học' },
];

export default function App() {
  const [tab, setTab] = useState('schedule');
  const [selectedClass, setSelectedClass] = useState(null);

  return (
    <div className="app">
      <header>
        <h1>🎓 LMS Management</h1>
        <nav>
          {TABS.map(t => (
            <button key={t.key} className={tab === t.key ? 'active' : ''} onClick={() => setTab(t.key)}>
              {t.label}
            </button>
          ))}
        </nav>
      </header>

      <main>
        {tab === 'schedule' && (
          <>
            <ClassSchedule onSelectClass={setSelectedClass} />
            <RegisterForm selectedClass={selectedClass} onClear={() => setSelectedClass(null)} />
            <CancelRegistration />
          </>
        )}

        {tab === 'people' && (
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
            <ParentForm />
            <StudentForm />
          </div>
        )}

        {tab === 'subscription' && <SubscriptionPanel />}
      </main>
    </div>
  );
}

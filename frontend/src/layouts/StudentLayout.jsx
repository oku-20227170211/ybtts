import { Outlet, Link, useNavigate } from 'react-router-dom'
import './StudentLayout.css'

function StudentLayout() {
  const navigate = useNavigate()

  const handleLogout = () => {
    localStorage.removeItem('studentToken')
    navigate('/login')
  }

  return (
    <div className="student-layout">
      <header className="student-header">
        <h1>Öğrenci Paneli</h1>
        <nav>
          <Link to="/student">Taleplerim</Link>
          <Link to="/student/create-request">Yeni Talep</Link>
          <button onClick={handleLogout}>Çıkış</button>
        </nav>
      </header>
      <main className="student-content">
        <Outlet />
      </main>
    </div>
  )
}

export default StudentLayout

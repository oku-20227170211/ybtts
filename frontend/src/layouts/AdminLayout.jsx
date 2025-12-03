import { Outlet, Link } from 'react-router-dom'
import './AdminLayout.css'

function AdminLayout() {
  return (
    <div className="admin-layout">
      <aside className="admin-sidebar">
        <h1>Admin Panel</h1>
        <nav>
          <Link to="/admin">Öğrenci Listesi</Link>
          <Link to="/admin/add-student">Öğrenci Ekle</Link>
        </nav>
      </aside>
      <main className="admin-content">
        <Outlet />
      </main>
    </div>
  )
}

export default AdminLayout

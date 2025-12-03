import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import AdminLayout from './layouts/AdminLayout'
import StudentLayout from './layouts/StudentLayout'
import StudentLogin from './pages/student/StudentLogin'
import StudentList from './pages/admin/StudentList'
import AddStudent from './pages/admin/AddStudent'
import CreateRequest from './pages/student/CreateRequest'
import MyRequests from './pages/student/MyRequests'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<StudentLogin />} />
        
        <Route path="/admin" element={<AdminLayout />}>
          <Route index element={<StudentList />} />
          <Route path="add-student" element={<AddStudent />} />
        </Route>

        <Route path="/student" element={<StudentLayout />}>
          <Route index element={<MyRequests />} />
          <Route path="create-request" element={<CreateRequest />} />
        </Route>

        <Route path="/" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App

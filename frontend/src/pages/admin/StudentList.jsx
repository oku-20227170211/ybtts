import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import api from '../../api/axios'
import './StudentList.css'

function StudentList() {
  const [students, setStudents] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    fetchStudents()
  }, [])

  const fetchStudents = async () => {
    try {
      const response = await api.get('/api/student')
      setStudents(response.data)
    } catch (err) {
      console.error('Öğrenciler yüklenemedi:', err)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Bu öğrenciyi silmek istediğinizden emin misiniz?')) {
      try {
        await api.delete(`/api/student/${id}`)
        setStudents(students.filter(s => s.id !== id))
      } catch (err) {
        console.error('Silme hatası:', err)
      }
    }
  }

  if (loading) return <div>Yükleniyor...</div>

  return (
    <div className="student-list-container">
      <div className="student-list-header">
        <h1>Öğrenci Listesi</h1>
        <Link to="/admin/add-student" className="btn-add">Yeni Öğrenci Ekle</Link>
      </div>
      {students.length === 0 ? (
        <p>Henüz öğrenci kaydı bulunmamaktadır.</p>
      ) : (
        <table className="students-table">
          <thead>
            <tr>
              <th>Ad Soyad</th>
              <th>Öğrenci Numarası</th>
              <th>Talepler</th>
              <th>İşlem</th>
            </tr>
          </thead>
          <tbody>
            {students.map((student) => (
              <tr key={student.id}>
                <td>{student.fullName}</td>
                <td>{student.studentNumber}</td>
                <td>{student.requests?.length || 0}</td>
                <td>
                  <button className="btn-delete" onClick={() => handleDelete(student.id)}>Sil</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}

export default StudentList

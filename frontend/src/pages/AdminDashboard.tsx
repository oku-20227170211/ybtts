import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Navbar from '../components/Navbar'
import api from '../api/api'

type Student = {
  id: number
  fullName: string
  studentNumber: string
  // optional: requests count or array if backend returns it
  requests?: any[]
}

const AdminDashboard: React.FC = () => {
  const [students, setStudents] = useState<Student[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  // modal state
  const [isOpen, setIsOpen] = useState(false)
  const [fullName, setFullName] = useState('')
  const [studentNumber, setStudentNumber] = useState('')
  const [password, setPassword] = useState('')
  const [submitting, setSubmitting] = useState(false)

  const fetchStudents = async () => {
    setLoading(true)
    setError(null)
    try {
      const resp = await api.get('/api/students')
      setStudents(Array.isArray(resp.data) ? resp.data : []);
    } catch (err: any) {
      setError(err?.response?.data?.message ?? 'Öğrenciler yüklenirken hata oluştu.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchStudents()
  }, [])

  const openModal = () => {
    setFullName('')
    setStudentNumber('')
    setPassword('')
    setIsOpen(true)
  }

  const closeModal = () => setIsOpen(false)

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault()
    setSubmitting(true)
    try {
      const payload = {
        fullName: fullName.trim(),
        studentNumber: studentNumber.trim(),
        password,
      }
      await api.post('/api/students', payload)
      await fetchStudents()
      closeModal()
    } catch (err: any) {
      setError(err?.response?.data?.message ?? 'Öğrenci oluşturulamadı.')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <>
      <Navbar />
      <div className="p-6 max-w-6xl mx-auto">
        <div className="flex items-center justify-between mb-6">
          <h1 className="text-2xl font-semibold text-gray-800">Yönetici Paneli</h1>
          <div className="flex gap-4">
            <Link 
              to="/admin/requests"
              className="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
            >
              📋 Talep Yönetimi
            </Link>
            <button
              onClick={openModal}
              className="inline-flex items-center px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700 transition"
            >
              + Öğrenci Ekle
            </button>
          </div>
        </div>

        {error && (
          <div className="mb-4 text-red-600">
            {error}
          </div>
        )}

        {loading ? (
          <div className="text-gray-600">Loading students...</div>
        ) : (
          <div className="overflow-x-auto bg-white rounded shadow">
            <table className="w-full min-w-[640px]">
              <thead className="bg-gray-50">
                <tr>
                  <th className="text-left px-4 py-3 text-sm font-medium text-gray-600">ID</th>
                  <th className="text-left px-4 py-3 text-sm font-medium text-gray-600">Full Name</th>
                  <th className="text-left px-4 py-3 text-sm font-medium text-gray-600">Student Number</th>
                  <th className="text-left px-4 py-3 text-sm font-medium text-gray-600">Requests</th>
                </tr>
              </thead>
              <tbody>
                {students.length === 0 ? (
                  <tr>
                    <td colSpan={4} className="px-4 py-6 text-center text-gray-500">No students found.</td>
                  </tr>
                ) : (
                  students.map((s) => (
                    <tr key={s.id} className="border-t hover:bg-gray-50">
                      <td className="px-4 py-3 text-sm text-gray-700">{s.id}</td>
                      <td className="px-4 py-3 text-sm text-gray-700">{s.fullName}</td>
                      <td className="px-4 py-3 text-sm text-gray-700">{s.studentNumber}</td>
                      <td className="px-4 py-3 text-sm text-gray-700">{s.requests ? s.requests.length : 0}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Modal */}
      {isOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">
          <div className="bg-white rounded-lg w-full max-w-md mx-4 shadow-lg">
            <div className="px-6 py-4 border-b">
              <h2 className="text-lg font-semibold">Add Student</h2>
            </div>
            <form onSubmit={handleCreate} className="px-6 py-4 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Full Name</label>
                <input
                  value={fullName}
                  onChange={(e) => setFullName(e.target.value)}
                  required
                  className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="Jane Doe"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Student Number</label>
                <input
                  value={studentNumber}
                  onChange={(e) => setStudentNumber(e.target.value)}
                  required
                  className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="20201234"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Password</label>
                <input
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                  type="password"
                  className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="••••••••"
                />
              </div>

              <div className="flex justify-end gap-3">
                <button
                  type="button"
                  onClick={closeModal}
                  className="px-4 py-2 rounded border text-gray-700 hover:bg-gray-50"
                  disabled={submitting}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-60"
                  disabled={submitting}
                >
                  {submitting ? 'Creating...' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  )
}

export default AdminDashboard

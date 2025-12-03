import { useState, useEffect } from "react";
import { useAuth } from "../context/AuthContext";
import * as requestsApi from "../api/requests";
import type { Request } from "../api/requests";
import { useNavigate } from "react-router-dom";

const StudentRequests: React.FC = () => {
  const { state } = useAuth();
  const navigate = useNavigate();

  const [requests, setRequests] = useState<Request[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [showModal, setShowModal] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    title: "",
    description: ""
  });

  const studentId = state.user?.id;

  // Talepleri yükle
  const fetchRequests = async () => {
    if (!studentId) return;

    setLoading(true);
    setError("");

    try {
      const data = await requestsApi.getStudentRequests(studentId);
      setRequests(data);
    } catch (err: any) {
      setError(err?.response?.data?.message || "Talepler yüklenemedi");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, [studentId]);


  // ⭐ Yeni Talep Oluştur
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.title.trim() || !formData.description.trim()) {
      setError("Başlık ve açıklama gereklidir");
      return;
    }

    if (!studentId) {
      setError("Öğrenci ID bulunamadı");
      return;
    }

    setSubmitting(true);
    setError("");

    try {
      await requestsApi.createRequest(studentId, formData.title, formData.description);

      // Modal kapat
      setShowModal(false);

      // Form temizle
      setFormData({ title: "", description: "" });

      // Liste güncelle
      await fetchRequests();

      // ⭐ Öğrenci dashboard sayfasına yönlendir
      navigate("/student");

    } catch (err: any) {
      const msg = err?.response?.data?.message || "Talep oluşturulamadı";
      setError(msg);
    } finally {
      setSubmitting(false);
    }
  };


  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };


  const getStatusColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case "pending":
        return "bg-yellow-100 text-yellow-800 border border-yellow-300";
      case "approved":
        return "bg-green-100 text-green-800 border border-green-300";
      case "rejected":
        return "bg-red-100 text-red-800 border border-red-300";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("tr-TR", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    });
  };


  if (!studentId) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <p className="text-xl">Lütfen giriş yapın</p>
      </div>
    );
  }


  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-50 to-blue-50 p-6">
      <div className="max-w-6xl mx-auto">

        {/* Header */}
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold">Taleplerim</h1>

          <button
            onClick={() => setShowModal(true)}
            className="bg-blue-600 text-white px-6 py-3 rounded-lg shadow hover:bg-blue-700"
          >
            + Yeni Talep Oluştur
          </button>
        </div>

        {/* Error */}
        {error && (
          <div className="mb-4 bg-red-100 border-l-4 border-red-500 p-4">
            <p className="text-red-700">{error}</p>
          </div>
        )}

        {/* Loading */}
        {loading ? (
          <div className="text-center py-10">
            <div className="spinner border-b-2 border-blue-500 w-10 h-10 mx-auto animate-spin"></div>
          </div>
        ) : requests.length === 0 ? (
          <div className="bg-white p-10 rounded shadow text-center">
            <p className="text-gray-600">Henüz talebiniz yok.</p>
          </div>
        ) : (
          <div className="grid gap-5">
            {requests.map((request) => (
              <div
                key={request.id}
                className="bg-white p-6 rounded-lg shadow border-l-4 border-blue-500"
              >
                <div className="flex justify-between mb-3">
                  <h2 className="text-2xl font-semibold">{request.title}</h2>
                  <span className={`px-3 py-1 rounded-full ${getStatusColor(request.status)}`}>
                    {request.status === "Pending" && "Bekleniyor"}
                    {request.status === "Approved" && "Onaylandı"}
                    {request.status === "Rejected" && "Reddedildi"}
                  </span>
                </div>

                <p className="text-gray-700">{request.description}</p>
                <p className="text-gray-500 text-sm mt-2">{formatDate(request.createdAt)}</p>
              </div>
            ))}
          </div>
        )}
      </div>


      {/* ⭐ Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg w-full max-w-md shadow-xl">
            <h2 className="text-2xl font-bold mb-6">Yeni Talep Oluştur</h2>

            <form onSubmit={handleSubmit}>
              {/* Title */}
              <input
                type="text"
                name="title"
                placeholder="Talep Başlığı"
                value={formData.title}
                onChange={handleInputChange}
                className="w-full mb-4 p-3 border rounded"
                disabled={submitting}
                required
              />

              {/* Description */}
              <textarea
                name="description"
                placeholder="Açıklama"
                value={formData.description}
                onChange={handleInputChange}
                className="w-full mb-4 p-3 border rounded"
                rows={4}
                disabled={submitting}
                required
              />

              <div className="flex gap-4">
                <button
                  type="button"
                  className="flex-1 bg-gray-300 py-2 rounded"
                  disabled={submitting}
                  onClick={() => {
                    setShowModal(false);
                    setError("");
                    setFormData({ title: "", description: "" });
                  }}
                >
                  İptal
                </button>

                <button
                  type="submit"
                  className="flex-1 bg-blue-600 text-white py-2 rounded"
                  disabled={submitting}
                >
                  {submitting ? "Gönderiliyor..." : "Talep Oluştur"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default StudentRequests;

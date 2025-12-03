import { useState, useEffect } from "react";
import * as requestsApi from "../api/requests";
import type { Request } from "../api/requests";
import { 
  getAllRequests, 
  approveRequest, 
  rejectRequest, 
  deleteRequest 
} from "../api/requests";


const AdminRequests: React.FC = () => {
  const [requests, setRequests] = useState<Request[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [updatingId, setUpdatingId] = useState<number | null>(null);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  // Tüm talepleri yükle
  const fetchRequests = async () => {
    setLoading(true);
    setError("");

    try {
      const data = await requestsApi.getAllRequests();
      setRequests(data);
    } catch (err: any) {
      setError(err?.response?.data?.message || "Talepler yüklenemedi");
      console.error("Error fetching requests:", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);


  const handleDelete = async (requestId: number) => {
  if (!confirm("Bu talebi silmek istediğinize emin misiniz?")) return;

  setDeletingId(requestId);
  setError("");

  try {
    await requestsApi.deleteRequest(requestId);

    setRequests((prev) => prev.filter((req) => req.id !== requestId));

    setSuccess("Talep başarıyla silindi.");
    setTimeout(() => setSuccess(""), 2000);
  } catch (err: any) {
    setError(err?.response?.data?.message || "Talep silinirken hata oluştu");
  } finally {
    setDeletingId(null);
  }
};


  // Talep durumu güncelle
  const handleStatusChange = async (
  requestId: number,
  newStatus: "Pending" | "Approved" | "Rejected"
) => {
  setUpdatingId(requestId);
  setError("");

  try {
    if (newStatus === "Approved") {
      await approveRequest(requestId);
    } else if (newStatus === "Rejected") {
      await rejectRequest(requestId);
    } else {
      // Pending’e dönme endpoint'in yok → backend yazılabilir
      await requestsApi.updateStatus(requestId, newStatus);
    }

    // UI güncelle
    setRequests((prev) =>
      prev.map((req) =>
        req.id === requestId ? { ...req, status: newStatus } : req
      )
    );

    setSuccess("Talep durumu güncellendi");
    setTimeout(() => setSuccess(""), 1500);

  } catch (err: any) {
    console.error(err);
    setError(err?.response?.data?.message || "Sunucu hatası");
  } finally {
    setUpdatingId(null);
  }
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

  const getStatusDropdownColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case "pending":
        return "border-yellow-300 bg-yellow-50";
      case "approved":
        return "border-green-300 bg-green-50";
      case "rejected":
        return "border-red-300 bg-red-50";
      default:
        return "border-gray-300";
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("tr-TR", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    });
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-900 to-gray-800 p-6">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-white mb-2">Talep Yönetimi</h1>
          <p className="text-gray-400">Tüm öğrenci taleplerini görüntüle ve yönet</p>
        </div>

        {/* Success Message */}
        {success && (
          <div className="mb-6 bg-green-50 border-l-4 border-green-500 p-4 rounded">
            <p className="text-green-700 font-medium">{success}</p>
          </div>
        )}

        {/* Error Message */}
        {error && (
          <div className="mb-6 bg-red-50 border-l-4 border-red-500 p-4 rounded">
            <p className="text-red-700 font-medium">{error}</p>
          </div>
        )}

        {/* Loading State */}
        {loading ? (
          <div className="flex justify-center items-center py-12">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
          </div>
        ) : requests.length === 0 ? (
          <div className="bg-gray-800 rounded-lg shadow-lg p-12 text-center border border-gray-700">
            <p className="text-gray-300 text-lg">Henüz talep bulunmuyor</p>
          </div>
        ) : (
          <div className="overflow-x-auto rounded-lg shadow-xl border border-gray-700">
            <table className="w-full bg-gray-800 text-gray-100">
              <thead>
                <tr className="bg-gradient-to-r from-blue-600 to-purple-600 text-white">
                  <th className="px-6 py-4 text-left font-semibold">ID</th>
                  <th className="px-6 py-4 text-left font-semibold">Öğrenci Adı</th>
                  <th className="px-6 py-4 text-left font-semibold">Başlık</th>
                  <th className="px-6 py-4 text-left font-semibold">Açıklama</th>
                  <th className="px-6 py-4 text-left font-semibold">Durum</th>
                  <th className="px-6 py-4 text-left font-semibold">Tarihi</th>
                  <th className="px-6 py-4 text-left font-semibold">İşlemler</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-700">
                {requests.map((request) => (
                  <tr
                    key={request.id}
                    className="hover:bg-gray-700 transition duration-200"
                  >
                    <td className="px-6 py-4 font-semibold text-blue-400">
                      #{request.id}
                    </td>
                    <td className="px-6 py-4 text-white">
                      {request.student?.fullName || "Bilinmiyor"}
                    </td>
                    <td className="px-6 py-4 font-medium text-white">
                      {request.title}
                    </td>
                    <td className="px-6 py-4 text-gray-300 max-w-xs truncate">
                      {request.description}
                    </td>
                    <td className="px-6 py-4">
                      <span
                        className={`px-3 py-1 rounded-full text-sm font-semibold ${getStatusColor(
                          request.status
                        )}`}
                      >
                        {request.status === "Pending" && "Bekleniyor"}
                        {request.status === "Approved" && "Onaylandı"}
                        {request.status === "Rejected" && "Reddedildi"}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-gray-400">
                      {formatDate(request.createdAt)}
                    </td>
<td className="px-6 py-4">

  <select
    value={request.status}
    onChange={(e) =>
      handleStatusChange(
        request.id,
        e.target.value as "Pending" | "Approved" | "Rejected"
      )
    }
    disabled={updatingId === request.id}
    className={`px-3 py-2 rounded-lg font-semibold border-2 cursor-pointer transition 
      ${getStatusDropdownColor(request.status)} 
      text-gray-900 hover:opacity-80 disabled:opacity-50 disabled:cursor-not-allowed`}
  >
    <option value="Pending">Bekleniyor</option>
    <option value="Approved">Onaylandı</option>
    <option value="Rejected">Reddedildi</option>
  </select>

  <button
    onClick={() => handleDelete(request.id)}
    disabled={deletingId === request.id}
    className="ml-3 px-3 py-2 bg-red-600 hover:bg-red-700 text-white text-sm rounded-lg transition disabled:opacity-50"
  >
    {deletingId === request.id ? "Siliniyor..." : "Sil"}
  </button>

</td>

                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {/* Stats Footer */}
        {!loading && requests.length > 0 && (
          <div className="mt-8 grid grid-cols-3 gap-4">
            <div className="bg-yellow-900 bg-opacity-50 border border-yellow-700 rounded-lg p-4">
              <p className="text-yellow-300 text-sm font-semibold">Bekleniyor</p>
              <p className="text-3xl font-bold text-yellow-200 mt-2">
                {requests.filter((r) => r.status === "Pending").length}
              </p>
            </div>
            <div className="bg-green-900 bg-opacity-50 border border-green-700 rounded-lg p-4">
              <p className="text-green-300 text-sm font-semibold">Onaylandı</p>
              <p className="text-3xl font-bold text-green-200 mt-2">
                {requests.filter((r) => r.status === "Approved").length}
              </p>
            </div>
            <div className="bg-red-900 bg-opacity-50 border border-red-700 rounded-lg p-4">
              <p className="text-red-300 text-sm font-semibold">Reddedildi</p>
              <p className="text-3xl font-bold text-red-200 mt-2">
                {requests.filter((r) => r.status === "Rejected").length}
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminRequests;

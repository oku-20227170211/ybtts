import api from "./api";

export interface Request {
  id: number;
  studentId: number;
  title: string;
  description: string;
  status: "Pending" | "Approved" | "Rejected";
  createdAt: string;
  student?: {
    id: number;
    fullName: string;
    studentNumber: string;
  };
}

export interface CreateRequestPayload {
  studentId: number;
  title: string;
  description: string;
}

export interface UpdateStatusPayload {
  status: "Pending" | "Approved" | "Rejected";
}

/**
 * Yeni talep oluştur
 */
export const createRequest = async (
  studentId: number,
  title: string,
  description: string
): Promise<Request> => {
  const response = await api.post("/api/requests", {
    studentId,
    title,
    description,
  });
  return response.data.data;
};

/**
 * Öğrencinin kendi taleplerini getir
 */
export const getStudentRequests = async (studentId: number): Promise<Request[]> => {
  const response = await api.get(`/api/requests/student/${studentId}`);
  return response.data.data || [];
};

/**
 * Tüm talepleri getir (Admin)
 */
export const getAllRequests = async (): Promise<Request[]> => {
  const response = await api.get("/api/requests");
  return response.data.data || [];
};

/**
 * Talep durumu güncelle (Admin)
 */
export const updateStatus = async (
  requestId: number,
  status: "Pending" | "Approved" | "Rejected"
): Promise<Request> => {
  const response = await api.put(`/api/requests/${requestId}`, { status });
  return response.data.data;
};

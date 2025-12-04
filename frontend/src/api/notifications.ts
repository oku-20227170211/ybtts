import api from './api';

export const getStudentNotifications = async (studentId: number, unreadOnly: boolean = false) => {
  const response = await api.get(`/api/notifications/student/${studentId}?unreadOnly=${unreadOnly}`);
  return response.data;
};

export const getStaffNotifications = async (staffId: number, unreadOnly: boolean = false) => {
  const response = await api.get(`/api/notifications/staff/${staffId}?unreadOnly=${unreadOnly}`);
  return response.data;
};

export const markAsRead = async (notificationId: number) => {
  const response = await api.put(`/api/notifications/${notificationId}/read`);
  return response.data;
};

export const getUnreadCount = async (studentId: number) => {
  const response = await api.get(`/api/notifications/student/${studentId}/unread-count`);
  return response.data;
};
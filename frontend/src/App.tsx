import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Login from "./pages/Login";
import AdminDashboard from "./pages/AdminDashboard";
import AdminRequests from "./pages/AdminRequests";
import StudentDashboard from "./pages/StudentDashboard";
import StudentRequests from "./pages/StudentRequests";
import { useAuth } from "./context/AuthContext";

const RequireAuth: React.FC<{ children: React.ReactNode; allowedRole?: "Admin" | "Student" }> = ({ children, allowedRole }) => {
  const { state } = useAuth();

  if (!state.user) {
    return <Navigate to="/login" replace />;
  }

  if (allowedRole && state.user.role !== allowedRole) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};


function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />

          <Route
            path="/admin"
            element={
              <RequireAuth allowedRole="Admin">
                <AdminDashboard />
              </RequireAuth>
            }
          />

          <Route
            path="/admin/requests"
            element={
              <RequireAuth allowedRole="Admin">
                <AdminRequests />
              </RequireAuth>
            }
          />

          <Route
            path="/student"
            element={
              <RequireAuth allowedRole="Student">
                <StudentDashboard />
              </RequireAuth>
            }
          />

          <Route
            path="/student/"
            element={
              <RequireAuth allowedRole="Student">
                <StudentRequests />
              </RequireAuth>
            }
          />

          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;

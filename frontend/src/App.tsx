import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Login from "./pages/Login";
import AdminDashboard from "./pages/AdminDashboard";
import AdminRequests from "./pages/AdminRequests";
import StudentDashboard from "./pages/StudentDashboard";
import StudentRequests from "./pages/StudentRequests";
import { useAuth } from "./context/AuthContext";

const RequireAuth: React.FC<{ children: React.ReactNode; role?: string }> = ({ children, role }) => {
  const { state } = useAuth();

  if (!state.user) return <Navigate to="/login" replace />;

  if (role && state.user.role !== role) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          {/* Login */}
          <Route path="/login" element={<Login />} />

          {/* ADMIN */}
          <Route
            path="/admin"
            element={
              <RequireAuth role="Admin">
                <AdminDashboard />
              </RequireAuth>
            }
          />

          <Route
            path="/admin/requests"
            element={
              <RequireAuth role="Admin">
                <AdminRequests />
              </RequireAuth>
            }
          />

          {/* STUDENT */}
          <Route
            path="/student"
            element={
              <RequireAuth role="Student">
                <StudentDashboard />
              </RequireAuth>
            }
          />

          <Route
            path="/student/requests"
            element={
              <RequireAuth role="Student">
                <StudentRequests />
              </RequireAuth>
            }
          />

          {/* Default */}
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;

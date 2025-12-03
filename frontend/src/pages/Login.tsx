import { useState } from "react";
import api from "../api/api";   // <-- axios yerine bunu kullanıyoruz
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const Login: React.FC = () => {
  const [mode, setMode] = useState<"student" | "admin">("student");
  const [studentNumber, setStudentNumber] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const { setState } = useAuth();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const login = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      let resp;

      if (mode === "admin") {
        resp = await api.post("/api/auth/login", {
          username,
          password,
        });
      } else {
        resp = await api.post("/api/student-auth/login", {
          studentNumber,
          password,
        });
      }

      console.log("LOGIN RESPONSE:", resp.data); // <-- Debug için

      if (resp.data?.success === true) {
        const user = mode === "admin" ? resp.data.user : resp.data.student;
        const token = resp.data.token ?? null;

        setState({ user, token });

        navigate(mode === "admin" ? "/admin" : "/student");
      } else {
        console.error("Login başarısız, response:", resp.data);
        setError(resp.data?.message ?? "Giriş başarısız");
      }
    } catch (err: any) {
      console.error("Login error:", err);
      setError(err?.response?.data?.message ?? err?.message ?? "Sunucu hatası");
    }

    setLoading(false);
  };

  return (
    <div>
      {error && <div style={{ background: "red", color: "white" }}>{error}</div>}
      <form onSubmit={login}>
        {/* FORM ELEMANLARI BURADA */}
      </form>
    </div>
  );
};

export default Login;

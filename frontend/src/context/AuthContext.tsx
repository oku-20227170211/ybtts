import { createContext, useContext, useState, ReactNode } from "react";

export type UserInfo = {
  id: number;
  role: "Admin" | "Student";
  fullName?: string;
  studentNumber?: string;
};

type AuthState = {
  user: UserInfo | null;
  token: string | null;
};

type AuthContextType = {
  state: AuthState;
  setState: (s: AuthState) => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>({
    user: null,
    token: null,
  });

  return (
    <AuthContext.Provider value={{ state, setState }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
}

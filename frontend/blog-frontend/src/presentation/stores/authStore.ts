import { create } from 'zustand';
import { Usuario } from '@/domain/entities/Usuario';
import { tokenStorage } from '@/infrastructure/storage/tokenStorage';

interface AuthState {
  user: Usuario | null;
  isAuthenticated: boolean;
  setUser: (user: Usuario, token: string) => void;
  logout: () => void;
  loadFromStorage: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,
  setUser: (user, token) => {
    tokenStorage.set(token);
    localStorage.setItem('user', JSON.stringify(user));
    set({ user, isAuthenticated: true });
  },
  logout: () => {
    tokenStorage.remove();
    localStorage.removeItem('user');
    set({ user: null, isAuthenticated: false });
  },
  loadFromStorage: () => {
    const token = tokenStorage.get();
    const userStr = typeof window !== 'undefined' ? localStorage.getItem('user') : null;
    if (token && userStr) {
      const user = JSON.parse(userStr) as Usuario;
      set({ user, isAuthenticated: true });
    }
  },
}));

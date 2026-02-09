export const tokenStorage = {
  get(): string | null {
    if (typeof window === 'undefined') return null;
    return localStorage.getItem('token');
  },

  set(token: string): void {
    localStorage.setItem('token', token);
  },

  remove(): void {
    localStorage.removeItem('token');
  },
};

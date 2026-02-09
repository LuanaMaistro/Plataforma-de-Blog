export interface Usuario {
  id: number;
  nome: string;
  email: string;
}

export interface UsuarioAutenticado extends Usuario {
  token: string;
}

import { UsuarioAutenticado } from '../entities/Usuario';

export interface IAuthRepository {
  login(email: string, senha: string): Promise<UsuarioAutenticado>;
  register(nome: string, email: string, senha: string): Promise<void>;
}

import { IAuthRepository } from '@/domain/interfaces/IAuthRepository';
import { UsuarioAutenticado } from '@/domain/entities/Usuario';
import { api } from '../http/axiosClient';

export class AuthRepository implements IAuthRepository {
  async login(email: string, senha: string): Promise<UsuarioAutenticado> {
    const response = await api.post('/auth/login', { email, senha });
    return response.data;
  }

  async register(nome: string, email: string, senha: string): Promise<void> {
    await api.post('/auth/register', { nome, email, senha });
  }
}

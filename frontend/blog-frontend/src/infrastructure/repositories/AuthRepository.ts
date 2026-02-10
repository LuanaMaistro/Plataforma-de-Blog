import { IAuthRepository } from '@/domain/interfaces/IAuthRepository';
import { Usuario, UsuarioAutenticado } from '@/domain/entities/Usuario';
import { api } from '../http/axiosClient';

export class AuthRepository implements IAuthRepository {
  async login(email: string, senha: string): Promise<UsuarioAutenticado> {
    const response = await api.post('/auth/login', { email, senha });
    return response.data;
  }

  async register(nome: string, email: string, senha: string): Promise<void> {
    await api.post('/auth/register', { nome, email, senha });
  }

  async atualizarPerfil(nome: string, senhaAtual?: string, novaSenha?: string): Promise<Usuario> {
    const response = await api.put('/auth/profile', { nome, senhaAtual, novaSenha });
    return response.data;
  }
}

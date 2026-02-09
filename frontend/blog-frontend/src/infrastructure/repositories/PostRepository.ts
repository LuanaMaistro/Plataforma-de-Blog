import { IPostRepository } from '@/domain/interfaces/IPostRepository';
import { Postagem } from '@/domain/entities/Postagem';
import { api } from '../http/axiosClient';

export class PostRepository implements IPostRepository {
  async listar(apenasMinhas = false): Promise<Postagem[]> {
    const params = apenasMinhas ? { meus_posts: true } : {};
    const response = await api.get('/posts', { params });
    return response.data;
  }

  async obterPorId(id: number): Promise<Postagem> {
    const response = await api.get(`/posts/${id}`);
    return response.data;
  }

  async criar(titulo: string, conteudo: string): Promise<Postagem> {
    const response = await api.post('/posts', { titulo, conteudo });
    return response.data;
  }

  async atualizar(id: number, titulo: string, conteudo: string): Promise<Postagem> {
    const response = await api.put(`/posts/${id}`, { titulo, conteudo });
    return response.data;
  }

  async deletar(id: number): Promise<void> {
    await api.delete(`/posts/${id}`);
  }
}

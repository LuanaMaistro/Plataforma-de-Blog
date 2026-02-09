import { Postagem } from '../entities/Postagem';

export interface IPostRepository {
  listar(apenasMinhas?: boolean): Promise<Postagem[]>;
  obterPorId(id: number): Promise<Postagem>;
  criar(titulo: string, conteudo: string): Promise<Postagem>;
  atualizar(id: number, titulo: string, conteudo: string): Promise<Postagem>;
  deletar(id: number): Promise<void>;
}

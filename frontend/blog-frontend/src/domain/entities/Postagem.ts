export interface Postagem {
  id: number;
  titulo: string;
  conteudo: string;
  dataCriacao: string;
  dataAtualizacao?: string;
  autor: {
    id: number;
    nome: string;
  };
}

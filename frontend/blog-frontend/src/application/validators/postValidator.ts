import { z } from 'zod';

export const postValidator = z.object({
  titulo: z.string()
    .min(1, 'Título é obrigatório')
    .max(200, 'Título deve ter no máximo 200 caracteres'),
  conteudo: z.string()
    .min(1, 'Conteúdo é obrigatório'),
});

export type PostFormData = z.infer<typeof postValidator>;

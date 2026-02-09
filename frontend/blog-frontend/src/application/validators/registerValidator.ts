import { z } from 'zod';

export const registerValidator = z.object({
  nome: z.string()
    .min(3, 'Nome deve ter pelo menos 3 caracteres'),
  email: z.string()
    .min(1, 'Email é obrigatório')
    .email('Email inválido'),
  senha: z.string()
    .min(6, 'Senha deve ter pelo menos 6 caracteres'),
});

export type RegisterFormData = z.infer<typeof registerValidator>;

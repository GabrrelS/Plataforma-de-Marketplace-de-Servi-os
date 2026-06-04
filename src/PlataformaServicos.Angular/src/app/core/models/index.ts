export interface Cliente {
  id?: number;
  nome: string;
  email: string;
  telefone: string;
}

export interface Prestador {
  id?: number;
  nome: string;
  email: string;
  especialidade: string;
  notaMedia?: number;
}

export interface Proposta {
  id?: number;
  titulo: string;
  descricao: string;
  valor: number;
  status?: 'Pendente' | 'Aceita' | 'Recusada';
  clienteId: number;
  prestadorId: number;
}

export interface Contrato {
  id?: number;
  dataInicio?: string;
  status?: string;
  clienteId: number;
  prestadorId: number;
  propostaId: number;
}

export interface Avaliacao {
  id?: number;
  nota: number;
  comentario: string;
  contratoId: number;
}

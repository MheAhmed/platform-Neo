import api from './api';

export interface PendingRegistration {
    id: string;
    nomEntreprise: string;
    matriculeFiscale?: string;
    nomResponsable: string;
    emailResponsable: string;
    statut: number;
    creeLe: string;
}

export const adminService = {
    async getPendingRegistrations(): Promise<PendingRegistration[]> {
        const response = await api.get<any[]>('/admin/pending');
        // Handle potential naming differences (PascalCase vs camelCase)
        return response.data.map(item => ({
            id: item.Id || item.id,
            nomEntreprise: item.NomEntreprise || item.nomEntreprise,
            matriculeFiscale: item.MatriculeFiscale || item.matriculeFiscale,
            nomResponsable: item.NomResponsable || item.nomResponsable,
            emailResponsable: item.EmailResponsable || item.emailResponsable,
            statut: item.Statut ?? item.statut,
            creeLe: item.CreeLe || item.creeLe
        }));
    },

    async approveRegistration(id: string): Promise<{ Message: string, TechnicalError?: string, FallbackLink?: string }> {
        const response = await api.post<{ Message: string, TechnicalError?: string, FallbackLink?: string }>(`/admin/approve/${id}`);
        return response.data;
    },

    async rejectRegistration(id: string, reason: string): Promise<{ Message: string }> {
        const response = await api.post<{ Message: string }>(`/admin/reject/${id}`, { reason });
        return response.data;
    }
};

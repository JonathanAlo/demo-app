export interface User {
    id: string
    uuid: string;
    firstname: string;
    lastname: string;
    email: string;
    role: string;
    image?: string;
    obsrvation?: string;
    file?: string;
    created_at?: string;
    updated_at?: string;
}

import localforage from "localforage";

const itemName = "tenants";

export type Tenant = TenantData & {
    id: string;
};

export type TenantData = {
    name: string;
    country: string;
};

export async function getTenants(): Promise<Tenant[]> {
    const response = await fetch("/tenant");
    const tenants = response.ok ? await response.json() : [];
    await set(tenants);
    return tenants;
}

export async function createTenant(tenantData: TenantData) {
    const headers = new Headers();
    const contentTypeJson = "application/json";
    headers.set("Accept", contentTypeJson);
    headers.set("Content-Type", contentTypeJson);
    const response = await fetch("/tenant", { method: "POST", body: JSON.stringify(tenantData), headers });
    const tenant = await response.json();
    const tenants = await getTenants();
    tenants.unshift(tenant);
    await set(tenants);
    return tenant;
}

export async function updateTenant(id: string, tenant: TenantData) {
    const headers = new Headers();
    const contentTypeJson = "application/json";
    headers.set("Accept", contentTypeJson);
    headers.set("Content-Type", contentTypeJson);
    await fetch(`/tenant/${id}`, { method: "PUT", body: JSON.stringify(tenant), headers });
    const tenants = await localforage.getItem<Tenant[]>(itemName) ?? [];
    const existing = tenants.find(t => t.id === id)!;
    existing.name = tenant.name;
    existing.country = tenant.country;
    await set(tenants);
    return tenant;
}

export async function getTenant(id: string) {
    const tenants = await localforage.getItem<Tenant[]>(itemName) ?? [];
    return tenants.find(t => t.id === id);
}

export async function deleteTenant(id: string) {
    await fetch(`/tenant/${id}`, { method: "DELETE" });
    const tenants = await localforage.getItem<Tenant[]>(itemName) ?? [];
    const index = tenants.findIndex(t => t.id === id);
    if (index > -1) {
        tenants.splice(index, 1);
        await set(tenants);
        return true;
    }
    return false;
}

function set(tenants: Tenant[]) {
    return localforage.setItem(itemName, tenants);
}
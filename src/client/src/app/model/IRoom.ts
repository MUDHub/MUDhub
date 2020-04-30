export interface IRoom {
	id?: string;
	name: string;
	enterMessage?: string;
	description?: string;
	position: {
		x: number;
		y: number;
	};
	connections: {
		n?: boolean;
		s?: boolean;
		e?: boolean;
		w?: boolean;
	};
}

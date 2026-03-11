function Heading({ title, text }: { title?: string; text?: string }) {
    return <h1 className="heading">{title || text}</h1>;
}

export default Heading;